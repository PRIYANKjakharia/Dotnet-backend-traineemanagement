import { Component,signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { LoginRequest } from '../../../shared/models/login-request';
import { Router } from '@angular/router';
// import { ChangeDetectorRef } from '@angular/core';
// import { NgZone } from '@angular/core';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  loginForm: FormGroup;
  errorMessage = signal('');

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    // private cdr : ChangeDetectorRef,
    // private zone: NgZone
  ) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }
    this.errorMessage.set('');
    const request: LoginRequest = this.loginForm.value as LoginRequest;

    this.authService.login(request).subscribe({
      next: (response) => {

        console.log(response);

        this.authService.setToken(response.token);
        this.router.navigate(['/dashboard']);

      },
      error: (error) => {
        // this.zone.run(()=>{
        //   if(error.status === 401){
        //     this.errorMessage = "invalid username or password"
        //     // alert(this.errorMessage);
        //   }else{
        //     this.errorMessage = "Something went wrong pls try again "
        //   }
        //   console.error("error is " ,error);
        // });
        // console.log("in error call");
        // console.log(error);
        if(error.status === 401){
          this.errorMessage.set("invalid username or password");
          // alert(this.errorMessage);
        }else{
          this.errorMessage.set("Something went wrong pls try again ");
        }
        console.error("error is " ,error);



        // force ui update cuz it wasnt detecting changes
        // this.cdr.detectChanges();
      }
    });

  }

}