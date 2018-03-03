import { Component, OnInit } from '@angular/core';
import { SciLogoutService } from '@speak/ng-sc/logout';
import { SciAuthService } from '@speak/ng-sc/auth';
import {ActivatedRoute } from '@angular/router';
import { CalendarModule } from 'primeng/components/calendar/calendar';
import { SafeResourceUrl } from '@angular/platform-browser/src/security/dom_sanitization_service';
import { DomSanitizer, SafeUrl} from '@angular/platform-browser';
import { NgScService } from '@speak/ng-sc';

@Component({
  selector: 'app-bi-dashboard',
  templateUrl: './bi-dashboard.component.html',
  styleUrls: ['./bi-dashboard.component.css']
})
export class BiDashboardComponent implements OnInit {
  isNavigationShown = true;
  rangeDates: Date[];
  pbiUrl: SafeResourceUrl;

  constructor(
    public authService: SciAuthService,
    public logoutService: SciLogoutService,
    public route: ActivatedRoute,
    private sanitizer: DomSanitizer,
    private ngScService: NgScService
  ) { }

  ngOnInit() {
    const today = new Date();
    const month = today.getMonth();
    const year = today.getFullYear();
    const prevMonth = (month === 0) ? 11 : month - 1;
    const prevYear = (prevMonth === 11) ? year - 1 : year;
    const nextMonth = (month === 11) ? 0 : month + 1;
    const nextYear = (nextMonth === 0) ? year + 1 : year;
    const monthAgoDate = new Date();
    monthAgoDate.setDate(today.getDate() - 30);
    this.rangeDates = [monthAgoDate, today];

    this.loadChart();

  }

  loadChart() {
    let fromDate = this.rangeDates[0].toISOString();
    let toDate = this.rangeDates[1].toISOString();

    this.pbiUrl = this.sanitizer.bypassSecurityTrustResourceUrl('https://app.powerbi.com/view?r=eyJrIjoiMWM2OGFlZGQtMTZhZC00ZDYxLWE3OTgtNzU4NzU3ZDBkMTljIiwidCI6IjA3ZTYyMzgxLTU0ZjAtNDNhMS05MjhiLTJmMzBjNTQ5NDc1YSJ9&from=' + fromDate + '&to=' + toDate);
  }

}
