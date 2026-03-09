export interface IActor {
    id: string,
    firstName: string;
    lastName: string;
    age: number;
    rating: number;
    films: { filmId: string, filmTitle: string }[];
}

export interface IActorShort {
    id: string;
    firstName: string;
    lastName: string;
}