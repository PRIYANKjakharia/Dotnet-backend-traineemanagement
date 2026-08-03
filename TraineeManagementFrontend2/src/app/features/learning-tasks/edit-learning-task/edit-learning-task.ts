import { Component, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { LearningTaskService } from '../learning-task.service';

@Component({
  selector: 'app-edit-learning-task',
  imports: [ReactiveFormsModule],
  templateUrl: './edit-learning-task.html',
  styleUrl: './edit-learning-task.css'
})
export class EditLearningTask implements OnInit {

  constructor(
    private fb: FormBuilder,
    private learningTaskService: LearningTaskService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  learningTaskForm!: FormGroup;
  id = 0;

  loading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  ngOnInit(): void {

    this.learningTaskForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', Validators.required],
      expectedTechStack: ['', Validators.required],
      dueDate: ['', Validators.required],
      status: ['', Validators.required]
    });

    this.id = Number(this.route.snapshot.paramMap.get('id'));

    this.loading.set(true);

    this.learningTaskService.getById(this.id).subscribe({

      next: (response) => {

        this.learningTaskForm.patchValue({
          title: response.title,
          description: response.description,
          expectedTechStack: response.expectedTechStack,
          dueDate: response.dueDate.substring(0, 10),
          status: response.status
        });

        this.loading.set(false);

      },

      error: (error) => {

        if (error.status === 404) {
          this.errorMessage.set('Learning Task not found.');
        } else {
          this.errorMessage.set('Something went wrong.');
        }

        this.loading.set(false);

      }

    });

  }

  onSubmit(): void {

    this.errorMessage.set('');
    this.successMessage.set('');

    if (this.learningTaskForm.invalid) {
      this.learningTaskForm.markAllAsTouched();
      return;
    }

    this.loading.set(true);

    this.learningTaskService.update(this.id, this.learningTaskForm.value).subscribe({

      next: () => {

        this.successMessage.set('Learning Task updated successfully.');

        setTimeout(() => {
          this.router.navigate(['/learningtasks']);
        }, 1000);

      },

      error: (error) => {

        if (error.status === 400) {
          this.errorMessage.set(error.error.message ?? 'Validation failed');
        } else if (error.status === 404) {
          this.errorMessage.set(error.error.message ?? 'Learning Task not found');
        } else {
          this.errorMessage.set('Something went wrong.');
        }

        this.loading.set(false);

      }

    });

  }

  cancel(): void {
    this.router.navigate(['/learningtasks']);
  }

}