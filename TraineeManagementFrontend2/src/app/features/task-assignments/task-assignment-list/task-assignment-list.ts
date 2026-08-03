import { Component, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { TaskAssignmentService } from '../task-assignment.service';
import { TaskAssignment } from '../../../shared/models/task-assignment';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-task-assignment-list',
  imports: [RouterLink, DatePipe],
  templateUrl: './task-assignment-list.html',
  styleUrl: './task-assignment-list.css'
})
export class TaskAssignmentList implements OnInit {

  constructor(private taskAssignmentService: TaskAssignmentService) {}

  taskAssignments: TaskAssignment[] = [];

  loading = signal(false);

  errorMessage = '';

  ngOnInit(): void {

    this.loadTaskAssignments();

  }

  loadTaskAssignments(): void {

    this.loading.set(true);
    this.errorMessage = '';
    this.taskAssignments = [];

    this.taskAssignmentService.getAll().subscribe({

      next: (response) => {

        this.taskAssignments = response;
        this.loading.set(false);

      },

      error: (error) => {

        if (error.status === 401) {

          this.errorMessage = 'Unauthorized';

        } else if (error.status === 403) {

          this.errorMessage = 'Forbidden';

        } else {

          this.errorMessage = 'Something went wrong.';

        }

        this.loading.set(false);

      }

    });

  }

  deleteTaskAssignment(id: number): void {

    if (!confirm('Delete this Task Assignment?')) {
      return;
    }

    this.taskAssignmentService.delete(id).subscribe({

      next: () => {

        this.loadTaskAssignments();

      },

      error: () => {

        alert('Unable to delete Task Assignment.');

      }

    });

  }

}