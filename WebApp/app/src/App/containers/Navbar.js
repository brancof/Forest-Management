import React from 'react';
import axios from 'axios';
import {
    Link
  } from "react-router-dom";
import './Navbar.css';

  class Navbar extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: "Bearer " + this.props.token,
            notificacoesPorLer: this.props.user.notificacoesPorLer,
            notifs: ''
        };
            this.logoutClick = this.logoutClick.bind(this);
            this.loadNotifs = this.loadNotifs.bind(this);
            this.showNotifs = this.showNotifs.bind(this);
            this.notifsDropClick = this.notifsDropClick.bind(this);
        };

        componentDidMount()
        {
            this.loadNotifs();
        }

        async loadNotifs()
        {
            await axios.get('https://localhost:44301/' + this.props.accounttype + '/notificacoes', {
                params: {
                    Username: this.props.user.username
                },
                headers: {
                    "Authorization": this.state.auth
                }
            })
            .then(response => {
                this.setState({notifs: response.data});
                console.log(response);
            }) 
            .catch(response => {
                console.log(response);
            })
        }

        showNotifs()
        {
            return (this.state.notifs.length > 0 ? this.state.notifs.map((notif, index) =>
                <div key={notif.id}>
                    <a className="dropdown-item notif-text" href="#">{notif.conteudo}
                    <p className="notif-date">{notif.dataEmissao.replace("T"," - ")}</p></a>
                </div>
            ) : null)
        }

        logoutClick()
        {
            this.props.change.token('');
            this.props.change.user('');
            this.props.change.username('');
        }

        notifsDropClick()
        {
            if(this.state.notificacoesPorLer > 0){
                axios({
                    method: 'put',
                    url: 'https://localhost:44301/' + this.props.accounttype + '/notificacoes/ler',
                    data: JSON.stringify(this.props.user.username), 
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": this.state.auth
                    }
                })
                    .then(response => {
                        this.setState({notificacoesPorLer: 0});
                        console.log(this.response);
                    }) 
                    .catch(response => {
                        console.log(this.response);
                    })
            }
        }

        render() {
            return (
                <nav className="navbar fixed-top navbar-expand-sm navbar-dark bg-dark">
                    <a className="navbar-brand"> Gestão de Florestas </a>
                    <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="collapse navbar-collapse" id="navbarNavAltMarkup">
                        <div className="navbar-nav">
                            <Link to="/"><a className="nav-item nav-link">Página Inicial</a></Link>
                        </div>
                        <div className="navbar-nav ml-auto">
                            <li className="nav-item dropdown">
                                <a className="nav-link dropdown" href="#" id="navbarDropdown" role="button" onClick={this.notifsDropClick} data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    {this.state.notificacoesPorLer < 1 ? 
                                        <div className="notifsLidas">
                                        <svg className="bi bi-envelope-open" width="1em" height="1em" viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                          <path fill-rule="evenodd" d="M8 8.917l7.757 4.654-.514.858L8 10.083.757 14.43l-.514-.858L8 8.917z"/>
                                          <path fill-rule="evenodd" d="M6.447 10.651L.243 6.93l.514-.858 6.204 3.723-.514.857zm9.31-3.722L9.553 10.65l-.514-.857 6.204-3.723.514.858z"/>
                                          <path fill-rule="evenodd" d="M15 14V5.236a1 1 0 0 0-.553-.894l-6-3a1 1 0 0 0-.894 0l-6 3A1 1 0 0 0 1 5.236V14a1 1 0 0 0 1 1h12a1 1 0 0 0 1-1zM1.106 3.447A2 2 0 0 0 0 5.237V14a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V5.236a2 2 0 0 0-1.106-1.789l-6-3a2 2 0 0 0-1.788 0l-6 3z"/>
                                        </svg>
                                        </div>
                                    : 
                                        <div className="notifsPorLer">
                                        <svg className="bi bi-envelope-fill" width="1em" height="1em" viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                            <path fill-rule="evenodd" d="M.05 3.555A2 2 0 0 1 2 2h12a2 2 0 0 1 1.95 1.555L8 8.414.05 3.555zM0 4.697v7.104l5.803-3.558L0 4.697zM6.761 8.83l-6.57 4.027A2 2 0 0 0 2 14h12a2 2 0 0 0 1.808-1.144l-6.57-4.027L8 9.586l-1.239-.757zm3.436-.586L16 11.801V4.697l-5.803 3.546z"/>
                                        </svg> ({this.state.notificacoesPorLer}) </div> 
                                    }
                                </a>
                                <div className="dropdown-menu dropdown-menu-right" aria-labelledby="navbarDropdown">
                                    {this.showNotifs()}
                                </div>
                            </li>
                            <Link to="/propoption"><a className="nav-item nav-link">Opções</a></Link>
                            <Link to="/login"><a className="nav-item nav-link" onClick={this.logoutClick}>Logout</a></Link>
                        </div>                   
                    </div>
                </nav>
            );
        }
  }

  export default Navbar