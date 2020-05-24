import React from 'react';
import axios from 'axios';
import {
    Link
  } from "react-router-dom";

  class NavbarSup extends React.Component {
    constructor(props) {
        super(props);

            this.logoutClick = this.logoutClick.bind(this);
        };

        logoutClick()
        {
            this.props.change.password('');
            this.props.change.username('');
            this.props.change.user('');
        }

        render() {
            return (
                <nav className="navbar navbar-expand-sm navbar-dark bg-dark">
                    <a className="navbar-brand"> Gestão de Florestas </a>
                    <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="collapse navbar-collapse" id="navbarNavAltMarkup">
                        <div className="navbar-nav">
                            <Link to="/supervisores"><a className="nav-item nav-link">Página Inicial</a></Link>
                            <Link to="/supervisoreschange"><a className="nav-item nav-link">Alterar Terreno</a></Link>
                        </div>
                        <div className="navbar-nav ml-auto">
                            <Link to="/login"><a className="nav-item nav-link" onClick={this.logoutClick}>Logout</a></Link>
                        </div>                   
                    </div>
                </nav>
            );
        }
  }

  export default NavbarSup