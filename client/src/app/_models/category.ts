import { IRecipe } from './recipe';

export interface ICategory {
    id: number;
    name: string;
    recipes?: IRecipe[];
}