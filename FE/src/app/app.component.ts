import { Component } from '@angular/core';
import { PrimeNGConfig } from 'primeng/api';
import { MainService } from './Shared/hubs/main.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'EventRegistryManagement';
  constructor(primeConfig: PrimeNGConfig, mainService: MainService) {
    primeConfig.ripple = true;
  }
}
