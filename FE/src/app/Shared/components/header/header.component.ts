import { Component, Input, OnInit } from '@angular/core';
import { UserDto } from '../../models/Users/UserDto.model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  @Input('user') user: UserDto | undefined;

  constructor(private auth: AuthService) {}

  ngOnInit(): void {}

  public logout() {
    this.auth.logout();
  }
}
