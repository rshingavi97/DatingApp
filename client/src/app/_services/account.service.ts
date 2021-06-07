import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators'; 
import {UserDto} from '../_models/userdto';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl="https://localhost:5001/api/";
  private recentUserSource = new ReplaySubject<UserDto>(1);
  recentUser$=this.recentUserSource.asObservable();
  constructor(private objHttp: HttpClient) {
    console.log("3 AccountService is instantiating");
   }
  login(objModel: any){
    return this.objHttp.post<UserDto>(this.baseUrl+'account/login', objModel).pipe(
      map((response: UserDto) => {
        const user = response;
        if(user)
        {
          console.log("account service => login() has called and user credentails stored");
          localStorage.setItem('user',JSON.stringify(user));
          this.recentUserSource.next(user); // omit given USER as response when it subscribed next time. 
        }
        return response;
        } /* end of map's arrow function*/ )/* end of map */ 
        )/* end of pipe */
  } /* end of login */

  setCurrentUser(user: UserDto)
  {
    console.log("Account Service  => inside setCurrentUser");
    this.recentUserSource.next(user); // omit given USER as response when it subscribed next time. 
  }
  logout()
  {
    console.log('inside AccountService::logout()');
    localStorage.removeItem('user');
    this.recentUserSource.next(); // omit given USER as response when it subscribed next time. 
    
    console.log('recentuser of ServiceAccount component '+this.recentUser$);
  }

   register(objModel: any){
    return this.objHttp.post<UserDto>(this.baseUrl+'account/register', objModel).pipe(
      map((response: UserDto) => {
        const user = response;
        if(user)
        {
          console.log("account service => register() has called and user credentails stored");
          localStorage.setItem('user',JSON.stringify(user));
          this.recentUserSource.next(user); // emit given USER as response when it subscribed next time. 
        }
        return user; 
        /* if we dont return user here then RESPONSE object would be empty
        inside register() method of Register component where register() is subscribed.
        Practically, we dont need to return it here but since we are printing the RESPONSE
        inside console as console.log(response); hence returning it (for education purpose)
        */

      } /* end of map's arrow function*/ )/* end of map */ )/* end of pipe */
  } /* end of register */

}
 