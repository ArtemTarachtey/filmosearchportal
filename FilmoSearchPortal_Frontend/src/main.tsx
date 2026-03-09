import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { Auth0Provider } from '@auth0/auth0-react';
import './index.css'
import App from './App.tsx'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Auth0Provider
      domain="dev-q1mqk1e4at8veunv.us.auth0.com"
      clientId="1feMPNjXKrfXoZhdLD9YauTL4Zq0gN1m"
      authorizationParams={{
        redirect_uri: window.location.origin,
        audience: "https://my-api" 
      }}
    >
    <App />
    </Auth0Provider>
  </StrictMode>,
)
