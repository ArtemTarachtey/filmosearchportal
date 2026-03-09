export interface IFilm {
    id?: string;
    title: string;
    description: string;
    duration: number;
    actors: { actorId: string, actorName: string}[];
    reviews: { reviewId: string, reviewName: string}[];
} 
export interface IFilmShort {
    filmId: string;
    filmTitle: string;
} 

export interface IFilmResponse {
    id: string;
    title: string;
}