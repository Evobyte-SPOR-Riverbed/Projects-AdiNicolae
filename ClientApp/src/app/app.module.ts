// Modules
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CustomValidatorsModule } from './helpers/custom-validators.module';
import { JwtModule } from '@auth0/angular-jwt';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

// Components
import { AppComponent } from './app.component';
import { AccessDialogComponent } from './components/access-dialog/access-dialog.component';
import { CocktailCardComponent } from './components/cocktail-card/cocktail-card.component';
import { FooterComponent } from './components/footer/footer.component';
import { MaterialModule } from './material.module';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { WelcomeCardComponent } from './components/welcome-card/welcome-card.component';

// Pages
import { HomeComponent } from './pages/home/home.component';
import { NewHomeComponent } from './pages/new-home/new-home.component';
import { CounterComponent } from './pages/counter/counter.component';
import { FetchDataComponent } from './pages/fetch-data/fetch-data.component';

// Services
import { AuthGuardService } from './services/auth-guard.service';
import { CountryService } from './services/country.service'
import { DrawerService } from './services/drawer.service';
import { BearerInterceptor } from './helpers/bearer-interceptor.module';
import { FiltersDrawerComponent } from './components/filters-drawer/filters-drawer.component';
import { SettingsDialogComponent } from './components/settings-dialog/settings-dialog.component';

export function accessTokenGetter(): string | null {
  return localStorage.getItem("accessToken");
}

@NgModule({
  declarations: [
    AppComponent,
    AccessDialogComponent,
    CocktailCardComponent,
    FiltersDrawerComponent,
    FooterComponent,
    NavMenuComponent,
    HomeComponent,
    NewHomeComponent,
    CounterComponent,
    FetchDataComponent,
    SettingsDialogComponent,
    WelcomeCardComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    CustomValidatorsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'new-home', component: NewHomeComponent },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
    ]),
    JwtModule.forRoot({
      config: {
        tokenGetter: accessTokenGetter,
        allowedDomains: ["localhost:44421"],
        disallowedRoutes: []
      }
    }),
    BrowserAnimationsModule,
    MaterialModule
  ],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: BearerInterceptor, multi: true }, AuthGuardService, CountryService, DrawerService],
  bootstrap: [AppComponent]
})
export class AppModule { }
