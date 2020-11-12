import { Interface } from "readline";

export interface IComment {
    id: number;
    createdAt: Date;
    body: string;
    displayName: string;
    image: string;
}