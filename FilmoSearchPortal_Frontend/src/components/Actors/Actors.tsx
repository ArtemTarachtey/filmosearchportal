import { getData, handleSend, handleDelete } from '../../utils/apiService';
import { useState, useEffect, useRef } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { type IFilmResponse } from '../../types/IFilm';
import { type IActor } from '../../types/IActor';
import { FILM_URL, ACTOR_URL } from '../../utils/constants';

const EMPTY_ACTOR = { id: '', firstName: '', lastName: '', age: 0, rating: 0, films: [] }

function Actors() {
    const [dialogMode, setDialogMode] = useState<string | null>(null);
    const errorDialog = useRef<HTMLDialogElement>(null);
    const dialogRef = useRef<HTMLDialogElement>(null);
    const confirmDialog = useRef<HTMLDialogElement>(null);
    const [deletedActor, setDeletedActor] = useState<IActor | null>(null);
    const [selectedActor, setSelectedActor] = useState<IActor | null>(null);
    const { getAccessTokenSilently, isLoading } = useAuth0();
    const [films, setFilms] = useState<{ filmId: string; filmTitle: string}[]>([]);
    const [actors, setActors] = useState<IActor[]>([]);

    useEffect(() => {
        if (isLoading) return;
        const loadActors = async () => {
            const token = await getAccessTokenSilently();
            const loadedActors = await getData(ACTOR_URL, token);
            setActors(loadedActors);
        }
        loadActors();
    }, [getAccessTokenSilently, isLoading]);

    useEffect(() => {
        if (isLoading) return;
        const loadFilms = async () => {
            const token = await getAccessTokenSilently();
            const loadedFilms = await getData(FILM_URL, token);
            const shortFilmInfo = loadedFilms.map((f: IFilmResponse) => ({
                filmId: f.id,
                filmTitle: f.title
            }))
            setFilms(shortFilmInfo);
        }   
        loadFilms();
    }, [getAccessTokenSilently, isLoading])

     const handleEditClick = (actor: IActor) => {
        setDialogMode('Редактирование');
        setSelectedActor(actor);
        dialogRef.current?.showModal();
    }
    
    const handleDeleteClick = (actor: IActor) => {
        setDeletedActor(actor);
        confirmDialog.current?.showModal();
    }
    
    const handleCreateClick = () => {
        setDialogMode('Создание');
        setSelectedActor(EMPTY_ACTOR);
        dialogRef.current?.showModal();
    }

    const sendData = async (e: React.FormEvent) => {
            e.preventDefault()
            if (!selectedActor) return;
            const isValid = selectedActor.age >= 0 && selectedActor.age <= 200 && selectedActor.rating >= 0 && selectedActor.rating <= 10;
            if (!isValid) {
                errorDialog.current?.showModal(); 
                return;
            } else {
                const token = await getAccessTokenSilently();
                const actorDto = {
                    id: selectedActor.id,
                    firstName: selectedActor.firstName,
                    lastName: selectedActor.lastName,
                    age: selectedActor.age,
                    rating: selectedActor.rating,
                    filmIds: selectedActor.films.map(f =>f.filmId)
                };
                await handleSend(ACTOR_URL, actorDto, selectedActor, setActors, dialogMode!, token,  dialogRef);
            }
    }
    
    const deleteFilmData = async () => {
        if (!deletedActor || !deletedActor.id) return;
        const token = await getAccessTokenSilently();
        await handleDelete(ACTOR_URL, deletedActor.id, token, setActors, confirmDialog);
    }

    if (isLoading) return <div>Загрузка...</div>;

    return (
        <>
            <h1>Актеры!</h1>
            <div className='container'>
                {actors ? actors?.map((a: IActor) =>
                <div className='card' key={a.id}>
                    <div className='card-field name'>{a.firstName + " " + a.lastName}</div>
                    <div className='card-field age'>{a.age}</div>
                    <div className='card-field rating'>{a.rating}</div>
                    {a.films && <div className='card-field actors'>{a.films.map((a,i,ar) => i !== ar.length - 1 ? `${a.filmTitle}, ` : a.filmTitle)}</div> }
                    <div className='card-actions'>
                        <button onClick={() => handleEditClick(a)} className='edit-action'></button>
                        <button onClick={() => handleDeleteClick(a)} className='delete-action'></button>
                    </div>
                </div>
                ) : <div>Error...</div>}
            </div>
            <dialog ref={errorDialog} className='modal'>
                <h2>Ошибка!</h2>
                <div style={{ whiteSpace: 'pre-line', margin: '1rem' }}>{`Требования к данным:
                Возраст: от 0 до 200 лет.
                Рейтинг: от 0 до 10.`}
                </div>
                <button className='btn' type="button" onClick={() => errorDialog.current?.close()}>Отмена</button>
            </dialog>
            <dialog ref={confirmDialog} className='modal delete-modal'>
                <h2>Вы уверены что хотите удалить данного актера?</h2>
                <div className="modal-buttons">
                    <button className='btn' type="submit" onClick={() => deleteFilmData()}>Удалить</button>
                    <button className='btn' type="button" onClick={() => confirmDialog.current?.close()}>Отмена</button>
                </div>
            </dialog>
            <dialog ref={dialogRef} className="modal">
                {selectedActor && (
                 <form className="form" method="dialog" onSubmit={async (e) => sendData(e) }>
                        <h2>{dialogMode}</h2>
                        <label className='form-label' htmlFor="input-first_name">Имя:</label>
                        <input
                            id='input-first_name'
                            className='form-input'
                            type='text' 
                            value={selectedActor?.firstName} 
                            onChange={(e) => setSelectedActor({...selectedActor, firstName: e.target.value})}
                        />

                        <label className='form-label' htmlFor="input-last_name">Фамилия:</label>
                        <input
                            id='input-last_name'
                            className='form-input'
                            type='text' 
                            value={selectedActor?.lastName} 
                            onChange={(e) => setSelectedActor({...selectedActor, lastName: e.target.value})}
                        />

                        <label className='form-label' htmlFor="input-age">Возраст:</label>
                        <input 
                            id='input-age'
                            className='form-input'
                            type='number' 
                            value={selectedActor.age}
                            onChange={(e) => { const val = e.target.value; setSelectedActor({...selectedActor, age: val == "" ? 0 : parseInt(val)})}}
                        />
        
                        <label className='form-label' htmlFor="input-rating">Рейтинг:</label>
                        <input 
                            id='input-rating'
                            className='form-input'
                            type='number' 
                            value={selectedActor.rating}
                            onChange={(e) => { const val = e.target.value; setSelectedActor({...selectedActor, rating: val == "" ? 0 : parseInt(val)})}}
                        />        

                        <label className='form-label' htmlFor="input-films">Фильмы:</label>
                        <select 
                            multiple
                            id='input-films'
                            className='form-input'
                            style={{ height: '100px' }}
                            value={selectedActor.films.map(f => f.filmId)}
                            onChange={(e) => {
                                const options = Array.from(e.target.selectedOptions);
                                const selectedIds = options.map(opt => opt.value);
                                const updatedFilms = films.filter(f => selectedIds.includes(f.filmId))
                                .map(f => ({
                                    
                                    filmId: f.filmId,
                                    filmTitle: f.filmTitle
                                }));
                                setSelectedActor({ ...selectedActor, films: updatedFilms });
                            }}
                        >
                            {films.map(f => (
                                <option key={f.filmId} value={f.filmId}>
                                    {f.filmTitle}
                                </option>
                            ))}
                        </select>
                        <div className="modal-buttons">
                            <button className='btn' type="submit">Сохранить</button>
                            <button className='btn' type="button" onClick={() => dialogRef.current?.close()}>Отмена</button>
                        </div>
                    </form>
                )}
            </dialog>
            <button className='add-btn' onClick={() => handleCreateClick()}>+</button>
        </>
    )
}

export default Actors;