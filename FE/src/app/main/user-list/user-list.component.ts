import { Component, OnInit } from '@angular/core';
import { BreadCrumbService } from 'src/app/Shared/services/client/bread-crumb.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  constructor(breadCrumb : BreadCrumbService) {
    breadCrumb.setPageTitle("Quản lý tài khoản")

   }

  ngOnInit(): void {
  }

}
