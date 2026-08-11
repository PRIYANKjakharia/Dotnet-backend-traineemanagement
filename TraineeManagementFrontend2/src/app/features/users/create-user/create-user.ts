import { Component, OnInit, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import {
  UserService,
  CreateUserRequest
} from '../user.service';

@Component({
  selector: 'app-create-user',
  imports: [ReactiveFormsModule],
  templateUrl: './create-user.html',
  styleUrl: './create-user.css'
})
export class CreateUser implements OnInit {

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  userForm!: FormGroup;

  role: 'mentor' | 'trainee' = 'trainee';

  loading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  ngOnInit(): void {

    const role = this.route.snapshot.queryParamMap.get('role');

    if (role === 'mentor' || role === 'trainee') {
      this.role = role;
      // console.log("here");
    } else {
      this.router.navigate(['/dashboard']);
      return;
    }

    this.userForm = this.fb.group({

      username: [
        '',
        [
          Validators.required,
          Validators.maxLength(50)
        ]
      ],

      email: [
        '',
        [
          Validators.required,
          Validators.email
        ]
      ],

      password: [
        '',
        [
          Validators.required
        ]
      ],

      firstName: [
        '',
        [
          Validators.required,
          Validators.maxLength(50)
        ]
      ],

      lastName: [
        '',
        [
          Validators.required,
          Validators.maxLength(50)
        ]
      ],

      techStack: [''],

      expertise: [''],

      status: [
        '',
        Validators.required
      ]

    });

    this.setRoleSpecificValidation();
  }

  setRoleSpecificValidation(): void {

    const techStack = this.userForm.get('techStack');
    const expertise = this.userForm.get('expertise');

    if (this.role === 'mentor') {

      expertise?.setValidators([
        Validators.required
      ]);

      techStack?.clearValidators();

    } else {

      techStack?.setValidators([
        Validators.required
      ]);

      expertise?.clearValidators();
    }

    techStack?.updateValueAndValidity();
    expertise?.updateValueAndValidity();
  }

  onSubmit(): void {

    this.errorMessage.set('');
    this.successMessage.set('');

    if (this.userForm.invalid) {

      this.userForm.markAllAsTouched();

      return;
    }

    this.loading.set(true);

    const request: CreateUserRequest = {
      username: this.userForm.value.username,
      email: this.userForm.value.email,
      password: this.userForm.value.password,
      role: this.role,
      firstName: this.userForm.value.firstName,
      lastName: this.userForm.value.lastName,
      status: this.userForm.value.status
    };

    if (this.role === 'mentor') {

      request.expertise = this.userForm.value.expertise;

    } else {

      request.techStack = this.userForm.value.techStack;

    }

    this.userService.create(request).subscribe({

      next: () => {

        this.successMessage.set(
          `${this.role === 'mentor' ? 'Mentor' : 'Trainee'} created successfully.`
        );

        setTimeout(() => {

          if (this.role === 'mentor') {
            this.router.navigate(['/mentors']);
          } else {
            this.router.navigate(['/trainees']);
          }

        }, 1000);

      },

      error: (error) => {

        if (error.status === 400) {

          this.errorMessage.set(
            error.error?.message ?? 'Validation failed'
          );

        } else if (error.status === 401) {

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

  cancel(): void {

    if (this.role === 'mentor') {
      this.router.navigate(['/mentors']);
    } else {
      this.router.navigate(['/trainees']);
    }

  }

}