import { HttpErrorResponse } from '@angular/common/http';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RouteMaps } from '../../constants/routes.constant';
import { UserDto } from '../../models/Users/UserDto.model';
import { AuthService } from '../../services/auth.service';
import { BreadCrumbService } from '../../services/client/bread-crumb.service';

declare var initLayout: any;

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css'],
})
export class LayoutComponent implements OnInit, AfterViewInit {
  public user!: UserDto;
  public title: string = 'Trang chủ';
  constructor(
    private auth: AuthService,
    private router: Router,
    private breadCrumb: BreadCrumbService
  ) {}
  ngAfterViewInit(): void {
    initLayout();
  }

  ngOnInit(): void {
    this.breadCrumb.pageTitle.subscribe((t) => (this.title = t));

    this.auth.user().subscribe({
      next: (res) => {
        if (res.data && res.success) {
          this.user = res.data;
        }
      },
      error: (err) => {
        if (err instanceof HttpErrorResponse) {
          this.router.navigateByUrl('auth/login');
        }
      },
    });
  }
}
