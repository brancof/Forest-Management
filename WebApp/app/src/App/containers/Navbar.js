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
            this.leftSideLinks = this.leftSideLinks.bind(this);
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

        leftSideLinks()
        {
            if(this.props.accounttype === "supervisores")
            {
                return(
                    <div className="navbar-nav">
                        <Link to="/" className="nav-item nav-link">Página Inicial</Link>
                        <Link to="/supervisoresChange"className="nav-item nav-link">Alterar Terreno</Link>
                        <Link to="/supervisoresMarcarInspecao" className="nav-item nav-link">Marcar Inspeção</Link>
                        <Link to="/supervisoresMarcarLimpeza" className="nav-item nav-link">Marcar Limpeza</Link>
                    </div>
                )
            }
            if(this.props.accounttype === "proprietarios")
            {
                return(
                    <div className="navbar-nav">
                        <Link to="/" className="nav-item nav-link">Página Inicial</Link>
                    </div>
                )
            }
            if(this.props.accounttype === "inspetores")
            {
                return(
                    <div className="navbar-nav">
                        <Link to="/" className="nav-item nav-link">Página Inicial</Link>
                    </div>
                )
            }
        }

        showNotifs()
        {
            return (this.state.notifs.length > 0 ? this.state.notifs.map((notif, index) =>
                <div key={notif.id}>
                    <a className="dropdown-item notif-text">{notif.conteudo}
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
                        {this.leftSideLinks()}
                        <div className="navbar-nav ml-auto">
                            <li className="nav-item dropdown">
                                <a className="nav-link dropdown" href="#" id="navbarDropdown" role="button" onClick={this.notifsDropClick} data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    {this.state.notificacoesPorLer < 1 ? 
                                        <div className="notifsLidas">
                                        <svg className="bi bi-envelope-open" width="1em" height="1em" viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                          <path fillRule="evenodd" d="M8 8.917l7.757 4.654-.514.858L8 10.083.757 14.43l-.514-.858L8 8.917z"/>
                                          <path fillRule="evenodd" d="M6.447 10.651L.243 6.93l.514-.858 6.204 3.723-.514.857zm9.31-3.722L9.553 10.65l-.514-.857 6.204-3.723.514.858z"/>
                                          <path fillRule="evenodd" d="M15 14V5.236a1 1 0 0 0-.553-.894l-6-3a1 1 0 0 0-.894 0l-6 3A1 1 0 0 0 1 5.236V14a1 1 0 0 0 1 1h12a1 1 0 0 0 1-1zM1.106 3.447A2 2 0 0 0 0 5.237V14a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V5.236a2 2 0 0 0-1.106-1.789l-6-3a2 2 0 0 0-1.788 0l-6 3z"/>
                                        </svg>
                                        </div>
                                    : 
                                        <div className="notifsPorLer">
                                        <svg className="bi bi-envelope-fill" width="1em" height="1em" viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                            <path fillRule="evenodd" d="M.05 3.555A2 2 0 0 1 2 2h12a2 2 0 0 1 1.95 1.555L8 8.414.05 3.555zM0 4.697v7.104l5.803-3.558L0 4.697zM6.761 8.83l-6.57 4.027A2 2 0 0 0 2 14h12a2 2 0 0 0 1.808-1.144l-6.57-4.027L8 9.586l-1.239-.757zm3.436-.586L16 11.801V4.697l-5.803 3.546z"/>
                                        </svg> ({this.state.notificacoesPorLer}) </div> 
                                    }
                                </a>
                                <div className="dropdown-menu dropdown-menu-right" aria-labelledby="navbarDropdown">
                                    {this.showNotifs()}
                                </div>
                            </li>
                            {this.props.accounttype === "proprietarios" ? 
                                <Link to="/propoption" className="nav-item nav-link">
                                    <svg className="bi bi-gear-fill" width="1em" height="1em" viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                        <path fillRule="evenodd" d="M9.405 1.05c-.413-1.4-2.397-1.4-2.81 0l-.1.34a1.464 1.464 0 0 1-2.105.872l-.31-.17c-1.283-.698-2.686.705-1.987 1.987l.169.311c.446.82.023 1.841-.872 2.105l-.34.1c-1.4.413-1.4 2.397 0 2.81l.34.1a1.464 1.464 0 0 1 .872 2.105l-.17.31c-.698 1.283.705 2.686 1.987 1.987l.311-.169a1.464 1.464 0 0 1 2.105.872l.1.34c.413 1.4 2.397 1.4 2.81 0l.1-.34a1.464 1.464 0 0 1 2.105-.872l.31.17c1.283.698 2.686-.705 1.987-1.987l-.169-.311a1.464 1.464 0 0 1 .872-2.105l.34-.1c1.4-.413 1.4-2.397 0-2.81l-.34-.1a1.464 1.464 0 0 1-.872-2.105l.17-.31c.698-1.283-.705-2.686-1.987-1.987l-.311.169a1.464 1.464 0 0 1-2.105-.872l-.1-.34zM8 10.93a2.929 2.929 0 1 0 0-5.86 2.929 2.929 0 0 0 0 5.858z"/>
                                    </svg>
                                </Link> : null}
                            <Link to="/login" className="nav-item nav-link" onClick={this.logoutClick}>
                            <svg className="bi bi-box-arrow-right" width="1em" height="1em" viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                <path fillRule="evenodd" d="M11.646 11.354a.5.5 0 0 1 0-.708L14.293 8l-2.647-2.646a.5.5 0 0 1 .708-.708l3 3a.5.5 0 0 1 0 .708l-3 3a.5.5 0 0 1-.708 0z"/>
                                <path fillRule="evenodd" d="M4.5 8a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 0 1H5a.5.5 0 0 1-.5-.5z"/>
                                <path fillRule="evenodd" d="M2 13.5A1.5 1.5 0 0 1 .5 12V4A1.5 1.5 0 0 1 2 2.5h7A1.5 1.5 0 0 1 10.5 4v1.5a.5.5 0 0 1-1 0V4a.5.5 0 0 0-.5-.5H2a.5.5 0 0 0-.5.5v8a.5.5 0 0 0 .5.5h7a.5.5 0 0 0 .5-.5v-1.5a.5.5 0 0 1 1 0V12A1.5 1.5 0 0 1 9 13.5H2z"/>
                            </svg>
                            </Link>
                        </div>                   
                    </div>
                </nav>
            );
        }
  }

  export default Navbar