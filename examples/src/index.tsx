import React, { useState } from 'react';
import ReactDOM from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { Root } from './Root';

const router = createBrowserRouter([
  {
    path: '/',
    element: <Root />,
    children: [
      {
        path: '/',
        element: <div>Hello world</div>
      }
    ]
  }
]);

const root = ReactDOM.createRoot(document.getElementById("root") as Element);
root.render(<React.StrictMode>
  <RouterProvider router={router} />
</React.StrictMode>);
