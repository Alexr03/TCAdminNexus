INSERT INTO tc_modules (module_id, display_name, version, enabled, config_page, component_directory, security_class)
VALUES ('6188e2ee-72f8-445d-9951-bce13326a6a0', 'Nexus', '2.0', 1, null, null, null);

-- ---------------------------------------------------------------------------------------------------------------------

INSERT INTO tc_site_map (page_id, module_id, parent_page_id, parent_page_module_id, category_id, url, mvc_url,
                         controller, action, display_name, page_small_icon, panelbar_icon, show_in_sidebar,
                         view_order, required_permissions, menu_required_permissions, page_manager,
                         page_search_provider, cache_name)
VALUES (1, '6188e2ee-72f8-445d-9951-bce13326a6a0', null, null, 1, '', '/Nexus', 'Nexus', 'Index', 'Nexus',
        'MenuIcons/Base/Info24x24.png', null, 1, null, '({07405876-e8c2-4b24-a774-4ef57f596384,0,8})',
        '({07405876-e8c2-4b24-a774-4ef57f596384,0,8})', null, null, '');

INSERT INTO tc_panelbar_categories (category_id, module_id, display_name, view_order, parent_category_id,
                                    parent_module_id, page_id, panelbar_icon)
VALUES (1, '6188e2ee-72f8-445d-9951-bce13326a6a0', 'Nexus Management', 1010, null, null, null, null);

-- ---------------------------------------------------------------------------------------------------------------------

INSERT INTO tc_server_enabled_components (module_id, component_id, server_id)
VALUES ('6188e2ee-72f8-445d-9951-bce13326a6a0', 1, 1);

INSERT INTO tc_module_server_components (module_id, component_id, display_name, short_name, description,
                                         component_type, visible, component_class, required, startup_order)
VALUES ('6188e2ee-72f8-445d-9951-bce13326a6a0', 1, 'Nexus Bot', 'nexus', 'Nexus Discord Bot', 1, 1,
        'TCAdminNexus.TcAdminModule, TCAdminNexus', 1, 100);

-- ---------------------------------------------------------------------------------------------------------------------

INSERT INTO tc_permission_categories (category_id, module_id, parent_category_id, parent_module_id,
                                      display_name, view_order)
VALUES (1, '6188e2ee-72f8-445d-9951-bce13326a6a0', null, null, 'Nexus', 1070);

INSERT INTO tc_permissions (permission_id, module_id, category_id, display_name, permission_type, view_order,
                            role_owner_required_permissions, same_role_required_permissions, top_level_only)
VALUES (1, '6188e2ee-72f8-445d-9951-bce13326a6a0', 1, 'View Statistics', 1, 1000, '', null, 1);
INSERT INTO tc_permissions (permission_id, module_id, category_id, display_name, permission_type, view_order,
                            role_owner_required_permissions, same_role_required_permissions, top_level_only)
VALUES (2, '6188e2ee-72f8-445d-9951-bce13326a6a0', 1, 'View Power Status', 1, 1001, '', null, 1);
INSERT INTO tc_permissions (permission_id, module_id, category_id, display_name, permission_type, view_order,
                            role_owner_required_permissions, same_role_required_permissions, top_level_only)
VALUES (3, '6188e2ee-72f8-445d-9951-bce13326a6a0', 1, 'Start/Stop', 1, 1002, '', null, 1);
INSERT INTO tc_permissions (permission_id, module_id, category_id, display_name, permission_type, view_order,
                            role_owner_required_permissions, same_role_required_permissions, top_level_only)
VALUES (4, '6188e2ee-72f8-445d-9951-bce13326a6a0', 1, 'Edit Configuration', 1, 1003, '', null, 1);
INSERT INTO tc_permissions (permission_id, module_id, category_id, display_name, permission_type, view_order,
                            role_owner_required_permissions, same_role_required_permissions, top_level_only)
VALUES (5, '6188e2ee-72f8-445d-9951-bce13326a6a0', 1, 'View Logs', 1, 1004, '', null, 1);