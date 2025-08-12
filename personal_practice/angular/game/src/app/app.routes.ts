import { Routes, RouterModule } from '@angular/router';
import { InnerStuffComponent } from './inner-stuff/inner-stuff.component';
import { NgModule } from '@angular/core';
import { errorComponent } from './error/error.component';
import { error1Component } from './error1/error1.component';
import { error2Component } from './error2/error2.component';
import { routeComponent } from './route/route.component';
import { rulesComponent } from './rules/rules.component';
import { HttpClientModule } from '@angular/common/http';

export const routes: Routes = [
  {path: '', redirectTo: '/route', pathMatch: 'full'},
  {path: 'game', component: InnerStuffComponent},
  {path: 'route', component: routeComponent},
  {path: 'error', component: errorComponent},
  {path: 'error1', component: error1Component},
  {path: 'error2', component: error2Component},
  {path: 'rules', component: rulesComponent}
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes), HttpClientModule],
  exports: [RouterModule]
})

export class AppRoutingModule {}
