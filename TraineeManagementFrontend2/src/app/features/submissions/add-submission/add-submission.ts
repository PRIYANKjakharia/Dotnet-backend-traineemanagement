import { Component, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { SubmissionService } from '../submission.service';
import { TaskAssignmentService } from '../../task-assignments/task-assignment.service';

import { TaskAssignment } from '../../../shared/models/task-assignment';

@Component({
  selector: 'app-add-submission',
  imports: [ReactiveFormsModule],
  templateUrl: './add-submission.html',
  styleUrl: './add-submission.css'
})
export class AddSubmission implements OnInit {

  constructor(
    private fb: FormBuilder,
    private submissionService: SubmissionService,
    private taskAssignmentService: TaskAssignmentService,
    private router: Router
  ) {}

  submissionForm!: FormGroup;

  taskAssignments = signal<TaskAssignment[]>([]);

  loading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  ngOnInit(): void {

    this.submissionForm = this.fb.group({

      taskAssignmentId: ['', Validators.required],
      submissionUrl: ['', Validators.required],
      notes: [''],
      submissionDate: ['', Validators.required],
      status: ['', Validators.required]

    });

    this.loadTaskAssignments();

  }

  loadTaskAssignments(): void {

    this.taskAssignmentService.getAll().subscribe({

      next: (res) => {

        this.taskAssignments.set(res);

      }

    });

  }

  onSubmit(): void {

    this.errorMessage.set('');
    this.successMessage.set('');

    if (this.submissionForm.invalid) {

      this.submissionForm.markAllAsTouched();
      return;

    }

    this.loading.set(true);

    this.submissionService.create(this.submissionForm.value).subscribe({

      next: () => {

        this.successMessage.set('Submission created successfully.');

        setTimeout(() => {

          this.router.navigate(['/submissions']);

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

    this.router.navigate(['/submissions']);

  }

}