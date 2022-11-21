import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Roles } from '../Shared/constants/constants.module';
import { RouteMaps } from '../Shared/constants/routes.constant';
import { AuthGuard } from '../Shared/guards/auth.guard';
import { MainComponent } from './main.component';
import { UserListComponent } from './user-list/user-list.component';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard],
    children: [
      {
        path: RouteMaps.users,
        component: UserListComponent,
        data: {
          Roles: [Roles.Admin],
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MainRoutingModule {}
