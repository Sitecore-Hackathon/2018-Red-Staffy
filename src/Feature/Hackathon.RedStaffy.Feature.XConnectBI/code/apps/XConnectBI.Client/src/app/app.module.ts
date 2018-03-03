import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';

import { ScAccountInformationModule } from '@speak/ng-bcl/account-information';
import { ScActionBarModule } from '@speak/ng-bcl/action-bar';
import { ScApplicationHeaderModule } from '@speak/ng-bcl/application-header';
import { ScButtonModule } from '@speak/ng-bcl/button';
import { ScGlobalHeaderModule } from '@speak/ng-bcl/global-header';
import { ScGlobalLogoModule } from '@speak/ng-bcl/global-logo';
import { ScIconModule } from '@speak/ng-bcl/icon';
import { ScMenuCategory, ScMenuItem, ScMenuItemLink, ScMenuModule } from '@speak/ng-bcl/menu';
import { ScTableModule } from '@speak/ng-bcl/table';
import { ScPageModule } from '@speak/ng-bcl/page';
import { CONTEXT, DICTIONARY } from '@speak/ng-bcl';
import { NgScModule } from '@speak/ng-sc';
import { RouterModule, Routes } from '@angular/router';
import { CalendarModule } from 'primeng/components/calendar/calendar';

import { AppComponent } from './app.component';
import { BiDashboardComponent } from './bi-dashboard/bi-dashboard.component';


@NgModule({
  declarations: [
    AppComponent,
    BiDashboardComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ScAccountInformationModule,
    ScActionBarModule,
    ScApplicationHeaderModule,
    ScButtonModule,
    ScGlobalHeaderModule,
    ScGlobalLogoModule,
    ScIconModule,
    ScPageModule,
    ScMenuModule,
    ScTableModule,
    CalendarModule,
    NgScModule.forRoot({
      contextToken: CONTEXT, // Provide Sitecore context for SPEAK 3 Components (optional)
      dictionaryToken: DICTIONARY, // Provide translations for SPEAK 3 Components (optional)
      translateItemId: '12990C27-0517-4EBD-A5A1-D0DE1BA41997', // ItemId where your application stores translation items (optional)
      authItemId: 'DB1EEE97-2DA8-428E-B6B6-A69BFECAA10E' // ItemId where your application stores user access authorization (optional)
    }),
    RouterModule.forRoot([
      {path: '', component: BiDashboardComponent, pathMatch: 'full'},
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
