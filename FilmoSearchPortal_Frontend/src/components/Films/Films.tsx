import { useRef, useState, useEffect } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { getData, handleDelete, handleSend } from '../../utils/apiService';
import { type IFilm } from '../../types/IFilm';
import { type IActorShort } from '../../types/IActor';
import { FILM_URL, ACTOR_URL } from '../../utils/constants';

const EMPTY_FILM = { title: '', description: '', duration: 0, actors: [], reviews: [] }

function Films() {
    const [dialogMode, setDialogMode] = useState<string | null>(null);
    const errorDialog = useRef<HTMLDialogElement>(null);
    const dialogRef = useRef<HTMLDialogElement>(null);
    const confirmDialog = useRef<HTMLDialogElement>(null);
    const [deletedFilm, setDeletedFilm] = useState<IFilm | null>(null);
    const [selectedFilm, setSelectedFilm] = useState<IFilm | null>(null);
    const [actors, setActors] = useState<{actorId: string, actorName: string}[]>([]);
    const [films, setFilms] = useState<IFilm[]>([]);
    const { getAccessTokenSilently, isLoading } = useAuth0();

    useEffect(() => {
    const loadActors = async () => {
        if (isLoading) return;
        const token = await getAccessTokenSilently();
        const data = await getData(ACTOR_URL, token);
        if (data) {
            const formatted = data.map((a: IActorShort) => ({
                actorId: a.id,
                actorName: `${a.firstName} ${a.lastName}`
            }));
            setActors(formatted);
        }
    }
        loadActors();
    }, [getAccessTokenSilently, isLoading]);

    useEffect(() => {
        const loadFilms = async () => {
            if (isLoading) return;
            const token: string = await getAccessTokenSilently();
            const data = await getData(FILM_URL, token)
            if (data) setFilms(data);
        }
        loadFilms();
    }, [getAccessTokenSilently, isLoading])

   

    const handleEditClick = (film: IFilm) => {
        setDialogMode('Редактирование');
        setSelectedFilm(film);
        dialogRef.current?.showModal();
    }

    const handleDeleteClick = (film: IFilm) => {
        setDeletedFilm(film);
        confirmDialog.current?.showModal();
    }

    const handleCreateClick = () => {
        setDialogMode('Создание');
        setSelectedFilm(EMPTY_FILM);
        dialogRef.current?.showModal();
    }

    const sendData = async (e: React.FormEvent) => {
        e.preventDefault()
        if (!selectedFilm) return;
        const isValid = selectedFilm.duration >= 0 && selectedFilm.duration <= 2000;
        if (!isValid) {
            errorDialog.current?.showModal();
            return;
        } else {
            const token = await getAccessTokenSilently();
            const filmDTO = {
                id: selectedFilm.id,
                title: selectedFilm.title,
                description: selectedFilm.description,
                duration: selectedFilm.duration,
                actorIds: selectedFilm.actors.map(a => a.actorId),
                reviewId: selectedFilm.reviews.map(f => f.reviewId),
            }
            await handleSend(FILM_URL, filmDTO, selectedFilm, setFilms, dialogMode!, token, dialogRef);
        }
    }  

    const deleteFilmData = async () => {
        if (!deletedFilm || !deletedFilm.id) return;
        const token = await getAccessTokenSilently();
        await handleDelete(FILM_URL, deletedFilm.id, token, setFilms, confirmDialog);
    }

     if (isLoading) return <div>Загрузка...</div>;

    return (
        <>
            <h1>Фильмы!</h1>
            <div className='container'>
                { films ? films.map(f => 
                <div className='card' key={f.id}>
                    <div className='card-field title'>{f.title}</div>
                    <div className='card-field description'>{f.description}</div>
                    <div className='card-field duration'>{f.duration}</div>
                    {f.actors && <div className='card-field actors'>{f.actors.map((f,i,a) => i !== a.length - 1 ? `${f.actorName}, ` : f.actorName)}</div> }
                    {f.reviews && <div className='card-field reviews'>{f.reviews.map((r,i,a) => i !== a.length -1 ? `${r.reviewName}, ` : r.reviewName)}</div>}
                    <div className='card-actions'>
                        <button onClick={() => handleEditClick(f)} className='edit-action'></button>
                        <button onClick={() => handleDeleteClick(f)} className='delete-action'></button>
                    </div>
                </div>
                ) : <div>Error...</div>}
            </div>
            <dialog ref={errorDialog} className='modal'>
                <h2>Ошибка!</h2>
                <p>Укажите длительность фильма в пределах от 0 до 2000 минут.</p>
                <button className='btn' type="button" onClick={() => errorDialog.current?.close()}>Отмена</button>
            </dialog>
            <dialog ref={confirmDialog} className='modal delete-modal'>
                <h2>Вы уверены что хотите удалить данный фильм?</h2>
                <div className="modal-buttons">
                    <button className='btn' type="submit" onClick={() => deleteFilmData()}>Удалить</button>
                    <button className='btn' type="button" onClick={() => confirmDialog.current?.close()}>Отмена</button>
                </div>
            </dialog>
            <dialog ref={dialogRef} className="modal">
                {selectedFilm && (
                 <form className="form" method="dialog" onSubmit={async (e) => sendData(e) }>
                        <h2>{dialogMode}</h2>
                        <label className='form-label' htmlFor="input-title">Название:</label>
                        <input
                            id='input-title'
                            className='form-input'
                            type='text' 
                            value={selectedFilm?.title} 
                            onChange={(e) => setSelectedFilm({...selectedFilm, title: e.target.value})}
                        />
                        
                        <label className='form-label' htmlFor="input-desc">Описание:</label>
                        <textarea 
                            id="input-desc"
                            className='form-input'
                            rows={4}
                            value={selectedFilm.description}
                            onChange={(e) => setSelectedFilm({...selectedFilm, description: e.target.value})}
                        />

                        <label className='form-label' htmlFor="input-duration">Длительность:</label>
                        <input 
                            id='input-duration'
                            className='form-input'
                            type='number' 
                            value={selectedFilm.duration}
                            onChange={(e) => { const val = e.target.value; setSelectedFilm({...selectedFilm, duration: val == "" ? 0 : parseInt(val)})}}
                        />

                        <label className='form-label' htmlFor="input-actors">Актеры:</label>
                        <select 
                            multiple
                            id='input-actors'
                            className='form-input'
                            style={{ height: '100px' }}
                            value={selectedFilm.actors.map(a => a.actorId)}
                            onChange={(e) => {
                                const options = Array.from(e.target.selectedOptions);
                                const selectedIds = options.map(opt => opt.value);
                                const updatedActors = actors
                                .filter(a => selectedIds.includes(a.actorId))
                                .map(a => ({
                                    actorId: a.actorId,
                                    actorName: a.actorName
                                }));
                                setSelectedFilm({ ...selectedFilm, actors: updatedActors });
                            }}
                        >
                            {actors.map(a => (
                                <option key={a.actorId} value={a.actorId}>
                                    {a.actorName}
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

export default Films;