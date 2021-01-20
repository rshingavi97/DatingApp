import { Component, OnInit } from '@angular/core';
import {HttpClient} from '@angular/common/http'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode: boolean=false;
  homeUsers: any; // App.component's users is nothing but homeUsers.
  /*homeUsers has the list of users sent by server and then, we are going to pass that list to the registerUsers i..e another variable of Register component.
  For the sake readability, i have given more descriptive names as homeUsers i..e users of home component
  or registerUsers i..e. users of register component
  */
  
  constructor(private objHttp: HttpClient) {
    console.log('constuctor of Home component');
   }

  ngOnInit(): void {
    console.log('ngOnInit of Home component');
   /* after implementing register() method inside AccountService , we do not need to fetch
   the user here by calling this.getUsers() hence commenting this line of code */
    //this.getUsers();
  }
  registerToggle(){
    this.registerMode = !this.registerMode;
  }

  /* Initially getUsers() method was calling from ngOnInit() but after implementing register() method inside AccountService , we do not need to fetch
   the user here by calling this.getUsers() hence commenting entire code */
   /*
  getUsers():void{
    this.objHttp.get('https://localhost:5001/api/users/')
    .subscribe(
              response=>{
                this.homeUsers = response;
              },
              error=>{
                console.log("Error generated at calling USERS API");
              });
  }
*/
  cancelRegisterMode(cancelRegisterEventFromRegisterComponent: boolean){
    this.registerMode = cancelRegisterEventFromRegisterComponent;
  }
}
