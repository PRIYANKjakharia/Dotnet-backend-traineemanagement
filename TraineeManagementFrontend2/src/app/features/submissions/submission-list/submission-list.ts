import { Component, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';

import { SubmissionService } from '../submission.service';
import { Submission } from '../../../shared/models/submission';

@Component({
  selector: 'app-submission-list',
  imports: [RouterLink, DatePipe],
  templateUrl: './submission-list.html',
  styleUrl: './submission-list.css'
})
export class SubmissionList implements OnInit {

  constructor(private submissionService: SubmissionService) {}

  submissions = signal<Submission[]>([]);
  loading = signal(false);
  errorMessage = signal('');

  ngOnInit(): void {
    this.loadSubmissions();
  }

  loadSubmissions(): void {

    this.loading.set(true);
    this.errorMessage.set('');

    this.submissionService.getAll().subscribe({

      next: (response) => {

        this.submissions.set(response);
        this.loading.set(false);

      },

      error: (error) => {

        if (error.status === 401) {

          this.errorMessage.set('Unauthorized');

        } else if (error.status === 403) {

          this.errorMessage.set('Forbidden');

        } else {

          this.errorMessage.set('Something went wrong.');

        }

        this.loading.set(false);

      }

    });

  }

}