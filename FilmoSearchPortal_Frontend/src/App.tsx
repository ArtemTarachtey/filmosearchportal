import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import { useAuth0 } from '@auth0/auth0-react'; 
import Home from './components/Home/Home';
import Films from './components/Films/Films';
import Actors from './components/Actors/Actors';
import Reviews from './components/Reviews/Reviews';
import './App.css';
import './global-styles.css';

function App() {
  const { isAuthenticated, loginWithRedirect } = useAuth0();

  if (!isAuthenticated) {
        return (
            <button className='auth-btn' onClick={() => loginWithRedirect()}>Авторизироваться...</button>
        )
  }
  
  return (
    <>
      <Router>
      <nav className='nav-menu'>
        <Link to="/">Домой</Link> | <Link to="/films">Фильмы</Link> | <Link to="/actors">Актеры</Link> | 
        <Link to="/reviews">Обзоры</Link>
      </nav>

      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/films" element={<Films />} />
        <Route path="/actors" element={<Actors />} />
        <Route path="/reviews" element={<Reviews />} />
      </Routes>
    </Router>
    </>
  )
}

export default App
