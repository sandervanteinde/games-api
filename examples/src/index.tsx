import React from 'react';
import ReactDOM from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { GamesLobby } from './games-lobby/GamesLobby';
import { Root } from './Root';

const router = createBrowserRouter([
  {
    path: '/',
    element: <Root />,
    children: [
      {
        path: '/',
        element: <GamesLobby/>
      }
    ]
  }
]);

const root = ReactDOM.createRoot(document.getElementById("root") as Element);
root.render(<React.StrictMode>
  <RouterProvider router={router} />
</React.StrictMode>);
