import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AppService } from 'src/app/services/app.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignUpComponent {
  singUpForm = new FormGroup({
    EmailAddress: new FormControl('', [Validators.required, Validators.email]),
    UserName: new FormControl('', Validators.required),
    Password: new FormControl('', Validators.required),
    DateCreated: new FormControl('')
  });

  constructor(private app: AppService) {}

  onSubmit() {
    if (this.singUpForm.valid) {
      this.singUpForm.patchValue({ DateCreated: new Date().toISOString() });

      // Send form data to the server using AppService
      this.app.signUp(this.singUpForm.value)
        .subscribe(
          (response) => {
            console.log('User added successfully:', response);
            // Reset the form
            this.singUpForm.reset();
          },
          (error) => {
            console.error('Error adding user:', error);
          }
        );
    } else {
      console.log('Form is invalid');
    }
  }
}
