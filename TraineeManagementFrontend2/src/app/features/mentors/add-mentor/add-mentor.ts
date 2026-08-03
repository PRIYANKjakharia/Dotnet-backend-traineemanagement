import { Component, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { MentorService } from '../mentor.service';

@Component({
  selector: 'app-add-mentor',
  imports: [ReactiveFormsModule],
  templateUrl: './add-mentor.html',
  styleUrl: './add-mentor.css'
})
export class AddMentor {

  constructor(
    private fb: FormBuilder,
    private mentorService: MentorService,
    private router: Router
  ) {}

  mentorForm!: FormGroup;

  errorMessage = signal('');
  successMessage = signal('');
  loading = signal(false);

  ngOnInit(): void {

    this.mentorForm = this.fb.group({

      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      expertise: ['', Validators.required],
      status: ['', Validators.required]

    });

  }

  onSubmit(): void {

    this.errorMessage.set('');
    this.successMessage.set('');

    if (this.mentorForm.invalid) {
      this.mentorForm.markAllAsTouched();
      return;
    }

    this.loading.set(true);

    this.mentorService.create(this.mentorForm.value).subscribe({

      next: () => {

        this.successMessage.set('Mentor added successfully.');

        setTimeout(() => {
          this.router.navigate(['/mentors']);
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
    this.router.navigate(['/mentors']);
  }

}