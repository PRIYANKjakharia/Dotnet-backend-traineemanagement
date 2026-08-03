import { Component, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { LearningTaskService } from '../learning-task.service';

@Component({
  selector: 'app-add-learning-task',
  imports: [ReactiveFormsModule],
  templateUrl: './add-learning-task.html',
  styleUrl: './add-learning-task.css'
})
export class AddLearningTask {

  constructor(
    private fb: FormBuilder,
    private learningTaskService: LearningTaskService,
    private router: Router
  ) {}

  learningTaskForm!: FormGroup;

  errorMessage = signal('');
  successMessage = signal('');
  loading = signal(false);

  ngOnInit(): void {

    this.learningTaskForm = this.fb.group({

      title: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', Validators.required],
      expectedTechStack: ['', Validators.required],
      dueDate: ['', Validators.required],
      status: ['', Validators.required]

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

    this.learningTaskService.create(this.learningTaskForm.value).subscribe({

      next: () => {

        this.successMessage.set('Learning Task added successfully.');

        setTimeout(() => {
          this.router.navigate(['/learningtasks']);
        }, 1000);

      },

      error: (error) => {

        if (error.status === 400) {
          this.errorMessage.set(error.error.message ?? 'Validation failed');
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