import { IMember } from './member';

export interface ILike {
    LikerId: number;  // user likes some1
    LikeeId: number; // user being like
    liker: IMember;
    likee: IMember;
  }