import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import {AccountService} from '../_services/account.service';
import {UserDto} from '../_models/userdto'

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit 
{

  loginModel:any ={} //this will hold the values of Login form.
   constructor(public objAccService:AccountService)
   { 
    
   }

  ngOnInit(): void
   {
    console.log("2 Nav component is loading and checking isUserActive");
  }
  login(){
      this.objAccService.login(this.loginModel).subscribe(
      objResponse=>{
      console.log(objResponse)
                },
      objError=>{
      console.log(objError)
                }
      );
    console.log("step 4"); //lets prints the values received from Login form.
  }
  logout(){
    this.objAccService.logout();
    console.log('inside AccountService::logout()');
   
  }

  
}
