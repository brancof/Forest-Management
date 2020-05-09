import React from 'react';
import axios from 'axios';
import {
    Link
  } from "react-router-dom";

  class Navbar extends React.Component {
    constructor(props) {
        super(props);

        };

        render() {
            return (
                <nav className="navbar navbar-expand-sm navbar-dark bg-dark">
                    <a className="navbar-brand"> Gestão de Florestas </a>
                    <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="collapse navbar-collapse" id="navbarNavAltMarkup">
                        <div className="navbar-nav">
                            <a className="nav-item nav-link">Opções</a>
                            <a className="nav-item nav-link">Logout</a>
                        </div>                   
                    </div>
                </nav>
            );
        }
  }

  export default Navbar