import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

/*const routes: Routes = [
  {path:'', component:HomeComponent},
  {path:'members', component:MemberListComponent, canActivate:[AuthGuard]},
  {path:'members/:id', component:MemberDetailComponent, canActivate:[AuthGuard]},
  {path:'lists', component:ListsComponent, canActivate:[AuthGuard]},
  {path:'messages', component:MessagesComponent, canActivate:[AuthGuard]},
  {path:'**', component:HomeComponent, pathMatch:'full'},
  ];
*/
  /*{path:'**', component:HomeComponent, pathMatch:'full'}
  In case for any irrelevant input inside the browser then HomeComponent will be loaded
  its like, DEFAULT switch case
  */

  //Refactoring of Routes array while adding AuthGuard
  const routes: Routes=[
    {path:'', component:HomeComponent},
    {
      path:'',
      runGuardsAndResolvers: 'always', //it means always run the guard while running
      canActivate:[AuthGuard], // what type of guard i.e. canActivate
      children:[
        {path:'members', component:MemberListComponent},
        {path:'members/:id', component:MemberDetailComponent},
        {path:'lists', component:ListsComponent},
        {path:'messages', component:MessagesComponent},
      ]
    },
    {path:'server-error', component:ServerErrorComponent},
    {path:'not-found', component:NotFoundComponent},
    {path:'errors', component:TestErrorsComponent},
    {path:'**', component:NotFoundComponent},
  ];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
