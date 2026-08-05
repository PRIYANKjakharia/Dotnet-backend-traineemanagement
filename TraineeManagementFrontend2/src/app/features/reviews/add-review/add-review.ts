import { Component, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { ReviewService } from '../review.service';
import { SubmissionService } from '../../submissions/submission.service';
import { MentorService } from '../../mentors/mentor.service';

import { Submission } from '../../../shared/models/submission';
import { Mentor } from '../../../shared/models/mentor';

@Component({
  selector: 'app-add-review',
  imports: [ReactiveFormsModule],
  templateUrl: './add-review.html',
  styleUrl: './add-review.css'
})
export class AddReview implements OnInit {

  constructor(
    private fb: FormBuilder,
    private reviewService: ReviewService,
    private submissionService: SubmissionService,
    private mentorService: MentorService,
    private router: Router
  ) {}

  reviewForm!: FormGroup;

  submissions = signal<Submission[]>([]);
  mentors = signal<Mentor[]>([]);

  loading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  ngOnInit(): void {

    this.reviewForm = this.fb.group({
      submissionId: ['', Validators.required],
      mentorId: ['', Validators.required],
      feedback: ['', Validators.required],
      reviewStatus: ['', Validators.required],
      score: ['', [Validators.required, Validators.min(0), Validators.max(100)]],
      reviewedDate: ['', Validators.required]

    });

    this.loadDropdowns();

  }

  loadDropdowns(): void {

    this.submissionService.getAll().subscribe({

      next: res => this.submissions.set(res)

    });

    this.mentorService.getAll().subscribe({

      next: res => this.mentors.set(res)

    });

  }

  onSubmit(): void {

    this.errorMessage.set('');
    this.successMessage.set('');

    if (this.reviewForm.invalid) {

      this.reviewForm.markAllAsTouched();
      return;

    }

    this.loading.set(true);

    this.reviewService.create(this.reviewForm.value).subscribe({

      next: () => {

        this.successMessage.set('Review created successfully.');

        setTimeout(() => {

          this.router.navigate(['/reviews']);

        },1000);

      },

      error: (error) => {

        if(error.status === 400){

          this.errorMessage.set(error.error.message ?? 'Validation failed');

        }else{

          this.errorMessage.set('Something went wrong.');

        }

        this.loading.set(false);

      }

    });

  }

  cancel(): void {

    this.router.navigate(['/reviews']);

  }

}