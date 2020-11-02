import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { getPaginatedResult, getPaginationHeaders } from '../_helpers/paginationHelper';
import { IMember } from '../_models/member';
import { UserParams } from '../_models/userParams';


@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: IMember[] = [];
  memberCache = new Map();

  constructor(private http: HttpClient, private memberService: MembersService) { }



  getMembers(userParams: UserParams) {
    var response = this.memberCache.get(Object.values(userParams).join('-'))
    if(response) {
      return of(response);
    }
    let params = getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    params = params.append('orderBy', userParams.orderBy);

    return getPaginatedResult<IMember[]>(this.baseUrl + 'users', params, this.http)
    .pipe(map(response => {
      this.memberCache.set(Object.values(userParams).join('-'), response);
      return response;
    }))
  }

  

  getMember(username: string) {
    const member = [...this.memberCache.values()]
    .reduce((previousValueAsArray, currentElement) => previousValueAsArray.concat(currentElement.result), [])
    .find((member: IMember) => member.username === username);

    if(member) {
      return of(member);
    }

    return this.http.get<IMember>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: IMember) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    )
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

  addLike(username: string) {
    return this.http.post(this.baseUrl + 'likes/' + username, {});
  }

  getLikes(predicate: string, pageNumber, pageSize) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('predicate', predicate);
    return getPaginatedResult<Partial<IMember[]>>(this.baseUrl + 'likes', params, this.http);
  }

}
