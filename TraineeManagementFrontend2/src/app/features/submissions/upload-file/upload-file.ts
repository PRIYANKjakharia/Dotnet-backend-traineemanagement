import { Component, OnInit, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { SubmissionService } from '../submission.service';

@Component({
  selector: 'app-upload-file',
  imports: [ReactiveFormsModule],
  templateUrl: './upload-file.html',
  styleUrl: './upload-file.css'
})
export class UploadFile implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private submissionService: SubmissionService
  ) {}

  submissionId = 0;

  selectedFile: File | null = null;

  loading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  ngOnInit(): void {

    this.submissionId = Number(this.route.snapshot.paramMap.get('id'));

  }

  onFileSelected(event: Event): void {

    const input = event.target as HTMLInputElement;

    if (input.files && input.files.length > 0) {

      this.selectedFile = input.files[0];

    }

  }

  upload(): void {

    this.errorMessage.set('');
    this.successMessage.set('');

    if (!this.selectedFile) {

      this.errorMessage.set('Please select a file.');
      return;

    }

    this.loading.set(true);

    this.submissionService.uploadFile(
      this.submissionId,
      this.selectedFile
    ).subscribe({

      next: () => {

        this.successMessage.set('File uploaded successfully.');

        setTimeout(() => {

          this.router.navigate(['/submissions']);

        },1000);

      },

      error: (error) => {

        if(error.status === 400){

          this.errorMessage.set(error.error.message);

        }else{

          this.errorMessage.set('Something went wrong.');

        }

        this.loading.set(false);

      }

    });

  }

}