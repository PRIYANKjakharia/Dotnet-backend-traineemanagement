import { Component, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { LearningTaskService } from '../learning-task.service';
import { LearningTask } from '../../../shared/models/learning-task';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-learning-task-list',
  imports: [RouterLink,DatePipe],
  templateUrl: './learning-task-list.html',
  styleUrl: './learning-task-list.css'
})
export class LearningTaskList implements OnInit {

  constructor(private learningTaskService: LearningTaskService) {}

  learningTasks: LearningTask[] = [];

  loading = signal(false);
  errorMessage = '';

  ngOnInit(): void {
    this.loadLearningTasks();
  }

  loadLearningTasks(): void {

    this.loading.set(true);
    this.errorMessage = '';
    this.learningTasks = [];

    this.learningTaskService.getAll().subscribe({

      next: (response) => {
        this.learningTasks = response;
        this.loading.set(false);
      },

      error: () => {
        this.errorMessage = 'Something went wrong.';
        this.loading.set(false);
      }

    });

  }

  deleteLearningTask(id: number): void {

    if (!confirm('Delete this learning task?')) {
      return;
    }

    this.learningTaskService.delete(id).subscribe({

      next: () => {
        this.loadLearningTasks();
      },

      error: (error) => {

        if (error.status === 404) {
          alert('Learning Task not found.');
        } else {
          alert('Unable to delete learning task.');
        }

      }

    });

  }

}