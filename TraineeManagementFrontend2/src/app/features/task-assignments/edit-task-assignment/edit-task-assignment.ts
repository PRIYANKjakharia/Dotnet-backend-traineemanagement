import { Component, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { TaskAssignmentService } from '../task-assignment.service';

@Component({
  selector: 'app-edit-task-assignment',
  imports: [ReactiveFormsModule],
  templateUrl: './edit-task-assignment.html',
  styleUrl: './edit-task-assignment.css'
})
export class EditTaskAssignment implements OnInit {

  constructor(
    private fb: FormBuilder,
    private taskAssignmentService: TaskAssignmentService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  taskAssignmentForm!: FormGroup;

  id = 0;

  loading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  ngOnInit(): void {

    this.taskAssignmentForm = this.fb.group({

      status: ['', Validators.required]

    });

    this.id = Number(this.route.snapshot.paramMap.get('id'));

    this.loading.set(true);

    this.taskAssignmentService.getById(this.id).subscribe({

      next: (response) => {

        this.taskAssignmentForm.patchValue({

          status: response.status

        });

        this.loading.set(false);

      },

      error: (error) => {

        if (error.status === 404) {

          this.errorMessage.set('Task Assignment not found.');

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

    if (this.taskAssignmentForm.invalid) {

      this.taskAssignmentForm.markAllAsTouched();
      return;

    }

    this.loading.set(true);

    this.taskAssignmentService.update(this.id, {

      id: this.id,
      status: this.taskAssignmentForm.value.status

    }).subscribe({

      next: () => {

        this.successMessage.set('Task Assignment updated successfully.');

        setTimeout(() => {

          this.router.navigate(['/taskassignments']);

        },1000);

      },

      error: (error) => {

        if(error.status === 404){

          this.errorMessage.set(error.error.message ?? 'Task Assignment not found');

        }else{

          this.errorMessage.set('Something went wrong.');

        }

        this.loading.set(false);

      }

    });

  }

  cancel(): void {

    this.router.navigate(['/taskassignments']);

  }

}