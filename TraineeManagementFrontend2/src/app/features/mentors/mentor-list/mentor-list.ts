import { Component, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { MentorService } from '../mentor.service';
import { Mentor } from '../../../shared/models/mentor';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-mentor-list',
  imports: [RouterLink],
  templateUrl: './mentor-list.html',
  styleUrl: './mentor-list.css'
})
export class MentorList implements OnInit {

  constructor(private mentorService: MentorService , public authService : AuthService) {}

  mentors: Mentor[] = [];

  loading = signal(false);
  errorMessage = '';

  ngOnInit(): void {
    this.loadMentors();
  }

  loadMentors(): void {

    this.loading.set(true);
    this.errorMessage = '';
    this.mentors = [];

    this.mentorService.getAll().subscribe({

      next: (response) => {

        this.mentors = response;
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

  // deleteMentor(id: number): void {

  //   if (!confirm('Delete this mentor?')) {
  //     return;
  //   }

  //   this.mentorService.delete(id).subscribe({

  //     next: () => {

  //       this.loadMentors();

  //     },

  //     error: () => {

  //       alert('Unable to delete mentor.');

  //     }

  //   });

  // }

}