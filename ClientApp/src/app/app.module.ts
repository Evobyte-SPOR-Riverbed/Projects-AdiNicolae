// Modules
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CustomValidatorsModule } from './helpers/custom-validators.module';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
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
import { CountryService } from './services/country.service'
import { DrawerService } from './services/drawer.service';

@NgModule({
  declarations: [
    AppComponent,
    AccessDialogComponent,
    CocktailCardComponent,
    FooterComponent,
    NavMenuComponent,
    HomeComponent,
    NewHomeComponent,
    CounterComponent,
    FetchDataComponent,
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
    BrowserAnimationsModule,
    MaterialModule
  ],
  providers: [CountryService, DrawerService],
  bootstrap: [AppComponent]
})
export class AppModule { }
