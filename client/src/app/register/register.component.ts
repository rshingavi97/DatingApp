import { Component, OnInit,Input, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
//import { EventEmitter } from 'events';
import {AccountService} from '../_services/account.service'
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();
  registerModel:any ={};
  @Input() registerUsers:any;
  
  constructor(private objAccServ:AccountService, private objToastSrv:ToastrService) { }

  ngOnInit(): void {
  }

  register(){
    this.objAccServ.register(this.registerModel).subscribe(
      (response)=>{
        console.log(response);
          /* after successful registration, dont display the Registration form hence remove it*/
        this.cancel();
      },
      (error)=>{
        console.log('error at registration ');
        console.log(error);
        this.objToastSrv.error(error.error);
      }
    )/* end of subscribe */
    
  }
  cancel(){
    this.cancelRegister.emit(false);
  }

}
