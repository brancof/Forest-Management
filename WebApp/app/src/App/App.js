import React from 'react';
import {
//  BrowserRouter,
//  Switch,
//  Route,
//  Link,
//  Redirect
} from "react-router-dom";
//import Login from './containers/Login';
//import Register from './containers/Register';
//import Proprietarios from './containers/Proprietarios';
//import Inspetores from './containers/Inspetores';
import AppRoutes from "./containers/AppRoutes";
import './App.css';



function App() {

  return (
    <div className="App">
      <AppRoutes />
    </div>
  )
}

export default App;


