import { Component, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TraineeService } from '../trainee.service';
// import { first } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-trainee',
  imports: [ReactiveFormsModule],
  templateUrl: './add-trainee.html',
  styleUrl: './add-trainee.css',
})
export class AddTrainee {
  constructor(private fb: FormBuilder , private traineeService: TraineeService , private router: Router){}
  traineeForm!: FormGroup;
  errorMessage = signal('');
  successMessage = signal('');
  loading = signal(false);
  ngOnInit(): void {
    this.traineeForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      techStack: ['', Validators.required],
      status: ['', Validators.required]
    });
  }
  onSubmit(): void {
    this.errorMessage.set('');
    this.successMessage.set('');
    if (this.traineeForm.invalid) {
      this.traineeForm.markAllAsTouched();
      return;
    }
    this.loading.set(true);

    this.traineeService.create(this.traineeForm.value).subscribe({
      next: (response) => {
        // console.log(response);
        this.successMessage.set('trainee added sucessfully');
        setTimeout(()=>{this.router.navigate(['/trainees'])} , 1000);
        this.loading.set(true);
      },
      error: (error) => {
        if(error.status === 400){
          this.errorMessage.set(error.error.message?? 'validation failed');
        }else{
          this.errorMessage.set('Something Went Wrong');
        }
        this.loading.set(false);
      }
    });
    // this.loading.set(false);

  }
  cancel(): void{
    this.router.navigate(['/trainees']);
  }
}
