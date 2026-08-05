import { Component, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';

import { ReviewService } from '../review.service';
import { Review } from '../../../shared/models/review';

@Component({
  selector: 'app-review-list',
  imports: [DatePipe, RouterLink],
  templateUrl: './review-list.html',
  styleUrl: './review-list.css'
})
export class ReviewList implements OnInit {

  constructor(private reviewService: ReviewService) {}

  reviews = signal<Review[]>([]);

  loading = signal(false);

  errorMessage = signal('');

  ngOnInit(): void {

    this.loadReviews();

  }

  loadReviews(): void {

    this.loading.set(true);
    this.errorMessage.set("");

    this.reviewService.getAll().subscribe({

      next: (response) => {

        this.reviews.set(response);
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