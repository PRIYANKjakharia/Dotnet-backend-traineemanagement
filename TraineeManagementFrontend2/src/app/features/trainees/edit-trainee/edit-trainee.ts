import { Component, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TraineeService } from '../trainee.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-edit-trainee',
  imports: [ReactiveFormsModule],
  templateUrl: './edit-trainee.html',
  styleUrl: './edit-trainee.css',
})
export class EditTrainee implements OnInit{
  constructor(
  private fb: FormBuilder,
  private traineeService: TraineeService,
  private route: ActivatedRoute,
  private router: Router ) {}

  traineeForm!: FormGroup;
  id = 0;

  loading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');
  ngOnInit():void{
    this.traineeForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      techStack: ['', Validators.required],
      status: ['', Validators.required]
    });
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    // console.log(this.id);
    this.loading.set(true);

    this.traineeService.getById(this.id).subscribe({
      next: (response) => {

        this.traineeForm.patchValue({
          firstName: response.firstName,
          lastName: response.lastName,
          email: response.email,
          techStack: response.techStack,
          status: response.status
        });

        this.loading.set(false);
      },
      error: (error) => {

        if (error.status === 404) {
          this.errorMessage.set('Trainee not found.');
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
    if (this.traineeForm.invalid) {
      this.traineeForm.markAllAsTouched();
      return;
    }
    this.loading.set(true);

    this.traineeService.update(this.id , this.traineeForm.value).subscribe({
      next: (response) => {
        // console.log(response);
        this.successMessage.set('trainee updated sucessfully');
        setTimeout(()=>{this.router.navigate(['/trainees'])} , 1000);
        this.loading.set(true);
      },
      error: (error) => {
        if(error.status === 400){
          this.errorMessage.set(error.error.message?? 'validation failed');
        }else if(error.status === 404){
          this.errorMessage.set(error.error.message?? 'Trainee not found');
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
