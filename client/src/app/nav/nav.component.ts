import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import {AccountService} from '../_services/account.service';
import {UserDto} from '../_models/userdto'
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit 
{
   loginModel:any ={} //this will hold the values of Login form.
   constructor(
     public objAccService:AccountService,
     private objRouter: Router,
     private objToastSrv:ToastrService){ }

  ngOnInit(): void {
  }
  login(){
      this.objAccService.login(this.loginModel).subscribe(
      objResponse=>{this.objRouter.navigateByUrl('/members');
     // this.objToastSrv.show('login() successful');
      this.objToastSrv.success('login successful');
    }/*,
      objError=>{ console.log(objError)
                  this.objToastSrv.error(objError.error); 
                 //here. ERROR is a property of objError internally
                }
      //ErrorInterceptor showing the message through Toaster service hence dont need above code
      */
  
      );
    console.log("step 4"); //lets prints the values received from Login form.
  }
  logout(){
    this.objAccService.logout();
    this.objRouter.navigateByUrl('/');
    console.log('inside AccountService::logout()');
   
  }

  
}
