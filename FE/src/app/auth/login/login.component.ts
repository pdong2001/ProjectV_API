import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserLoginRequestDto } from 'src/app/Shared/models/Users/UserLoginRequestDto.model';
import { AuthService } from 'src/app/Shared/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  public userLogin: UserLoginRequestDto;

  constructor(private router: Router, private authData: AuthService) {
    this.userLogin = {
      userName: 'admin',
      password: 'Admin@123',
      remember: true,
    };
  }

  ngOnInit(): void {}

  public login() {
    if (this.userLogin.userName && this.userLogin.password) {
      this.authData.login(this.userLogin).subscribe((res) => {
        if (res.data) {
          this.router.navigateByUrl('/');
        }
      });
    }
  }
}
