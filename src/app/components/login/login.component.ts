import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AppService } from 'src/app/services/app.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm = new FormGroup({
    EmailAddress: new FormControl('', [Validators.required, Validators.email]),
    Password: new FormControl('', Validators.required)
  });

  constructor(private app: AppService) {}

  onSubmit() {
    if (this.loginForm.valid) {
      console.log('Form Data:', this.loginForm.value);
      this.app.login(this.loginForm.value.EmailAddress, this.loginForm.value.Password) 
        .subscribe(
          (response) => {
            console.log('Login successful:', response);
            let jsonData = JSON.stringify(response.user);
            sessionStorage.setItem('user',jsonData);
            location.assign('http://localhost:4200');
          },
          (error) => {
            console.error('Login failed:', error);
          }
        );
    } else {
      console.log('Form is invalid');
    }
  }
}
