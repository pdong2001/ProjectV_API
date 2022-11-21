import { Injectable } from '@angular/core';
import { Observable, Subscriber } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BreadCrumbService {
  public pageTitle: Observable<string>;
  private pageTitleSubcriber: Subscriber<string> | undefined;
  constructor() {
    this.pageTitle = new Observable<string>(
      (s) => (this.pageTitleSubcriber = s)
    );
  }

  public setPageTitle(title: string) {
    if (this.pageTitleSubcriber) {
      this.pageTitleSubcriber.next(title);
    }
  }
}
