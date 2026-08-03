import { Component, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MentorService } from '../mentor.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-edit-mentor',
  imports: [ReactiveFormsModule],
  templateUrl: './edit-mentor.html',
  styleUrl: './edit-mentor.css',
})
export class EditMentor implements OnInit {

  constructor(
    private fb: FormBuilder,
    private mentorService: MentorService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  mentorForm!: FormGroup;
  id = 0;

  loading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  ngOnInit(): void {

    this.mentorForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      expertise: ['', Validators.required],
      status: ['', Validators.required]
    });

    this.id = Number(this.route.snapshot.paramMap.get('id'));

    this.loading.set(true);

    this.mentorService.getById(this.id).subscribe({

      next: (response) => {

        this.mentorForm.patchValue({
          firstName: response.firstName,
          lastName: response.lastName,
          email: response.email,
          expertise: response.expertise,
          status: response.status
        });

        this.loading.set(false);

      },

      error: (error) => {

        if (error.status === 404) {
          this.errorMessage.set('Mentor not found.');
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

    if (this.mentorForm.invalid) {
      this.mentorForm.markAllAsTouched();
      return;
    }

    this.loading.set(true);

    this.mentorService.update(this.id, this.mentorForm.value).subscribe({

      next: () => {

        this.successMessage.set('Mentor updated successfully.');

        setTimeout(() => {
          this.router.navigate(['/mentors']);
        }, 1000);

      },

      error: (error) => {

        if (error.status === 400) {
          this.errorMessage.set(error.error.message ?? 'Validation failed');
        } else if (error.status === 404) {
          this.errorMessage.set(error.error.message ?? 'Mentor not found');
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